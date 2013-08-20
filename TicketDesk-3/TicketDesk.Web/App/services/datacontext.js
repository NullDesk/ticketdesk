define([
    'durandal/system',
    'services/model',
    'config',
    'services/logger',
    'services/breeze.partial-entities'],
    function (system, model, config, logger, partialMapper) {
        var entityQuery = breeze.EntityQuery;
        var manager = configureBreezeManager();
        var orderBy = model.orderBy;
        var entityNames = model.entityNames;

        var getPriorityList = function (priorityListObservable, forceRemote) {
            return getSettingList('PriorityList', priorityListObservable, forceRemote);
        };

        var getTicketTypeList = function (ticketTypeListObservable, forceRemote) {
            return getSettingList('TicketTypeList', ticketTypeListObservable, forceRemote);
        };

        var getCategoryList = function (categoryListObservable, forceRemote) {
            return getSettingList('CategoryList', categoryListObservable, forceRemote);
        };

        var getStatusList = function (statusListObservable, forceRemote) {
            return getSettingList('StatusList', statusListObservable, forceRemote);
        };

        var ticketEntityManager = function () {
            var openTicketRowCount = ko.observable(0);
            var backgroudTicketRefreshCounter = ko.observable(0);
            
            var baseOpenTicketPartialQuery =
                entityQuery.from('Tickets')
                    .where("ticketStatus", "!=", "Closed")
                    .orderBy(orderBy.ticket);
            
            var getOpenTicketPagedList = function(ticketsObservable, forPageIndex, forceRemote) {
                if (forceRemote || openTicketRowCount() < 1) {
                    return fetchOpenTicketPartials().then(fetchLocal);
                } else {
                    return fetchLocal();
                }

                function fetchLocal() {
                    var query = baseOpenTicketPartialQuery.skip(forPageIndex * 5).take(5).using(breeze.FetchStrategy.FromLocalCache);
                    return manager.executeQuery(query)
                        .then(querySucceeded)
                        .fail(queryFailed);

                    function querySucceeded(data) {
                        if (data.results.length > 0) {
                            ticketsObservable(data.results);
                        }
                    }
                }
            };

            var fetchOpenTicketPartials = function() {
                var query = baseOpenTicketPartialQuery
                    .select('ticketId, title, ticketType, owner, assignedTo, ticketStatus, category, priority, createdBy, createdDate, lastUpdateBy, lastUpdateDate')
                    .using(breeze.FetchStrategy.FromServer).inlineCount(true);
                return manager.executeQuery(query)
                    .then(querySucceeded)
                    .fail(queryFailed);

                function querySucceeded(data) {
                    openTicketRowCount(data.inlineCount);
                    partialMapper.mapDtosToEntities(manager, data.results, entityNames.ticket, 'ticketId');
                    log('Retrieved [Tickets] from remote data source', data, true);
                }
            };
            
            var refreshTicketById = function (ticketId) {
               
                return manager.fetchEntityByKey(
                    entityNames.ticket, ticketId, true)
                    .then(localFetchSucceeded)
                    .fail(queryFailed);

                function localFetchSucceeded(data) {
                    log('refreshed Ticket ' + ticketId, data, true);
                    var wasOpen = data.entity.ticketStatus() !== 'Closed';
                    return entityQuery.fromEntities(data.entity)
                        .using(manager).execute().then(remoteFetchSucceeded);

                    function remoteFetchSucceeded(rdata) {
                        //the entire point here is to make sure we decriment/increment the 
                        //  count of cached partials to make sure paging doesn't break
                        //  when a ticket is closed or un-closed
                        if (rdata.results.length > 0) {
                            var isOpen = rdata.results[0].ticketStatus() !== 'Closed';
                            if (wasOpen !== isOpen) {
                                if (isOpen) {
                                    ticketEntityManager.openTicketRowCount(ticketEntityManager.openTicketRowCount() + 1);
                                } else {
                                    ticketEntityManager.openTicketRowCount(ticketEntityManager.openTicketRowCount() - 1);
                                }
                            }
                            //increment this to notify listeners that a new refresh occured
                            ticketEntityManager.backgroudTicketRefreshCounter(ticketEntityManager.backgroudTicketRefreshCounter() + 1);
                        }
                    }
                }
            };

            var createTicket = function () {
                return manager.createEntity(entityNames.ticket);
            };


            var ticketManager = {
                getOpenTicketPagedList: getOpenTicketPagedList,
                openTicketRowCount: openTicketRowCount,
                refreshTicketById: refreshTicketById,
                backgroudTicketRefreshCounter: backgroudTicketRefreshCounter,
                createTicket: createTicket
            };

            return ticketManager;
        }();

       
       

        var getTicketById = function (ticketId, ticketObservable) {

            // 1st - fetchEntityByKey will look in local cache 
            // first (because 3rd parm is true) 
            // if not there then it will go remote
            return manager.fetchEntityByKey(
                entityNames.ticket, ticketId, true)
                .then(fetchSucceeded)
                .fail(queryFailed);

            // 2nd - Refresh the entity from remote store (if needed)
            function fetchSucceeded(data) {
                var s = data.entity;
                return s.isPartial() ? refreshTicket(s) : ticketObservable(s);
            }

            function refreshTicket(ticket) {
                return entityQuery.fromEntities(ticket)
                    .using(manager).execute()
                    .then(querySucceeded)
                    .fail(queryFailed);
            }

            function querySucceeded(data) {
                var s = data.results[0];
                s.isPartial(false);

                log('Retrieved [Ticket] from remote data source', s, true);
                return ticketObservable(s);
            }

        };



        var cancelChanges = function () {
            manager.rejectChanges();
            log('Canceled changes', null, true);
        };

        var saveChanges = function () {
            return manager.saveChanges()
                .then(saveSucceeded)
                .fail(saveFailed);

            function saveSucceeded(saveResult) {
                log('Saved data successfully', saveResult, true);
            }

            function saveFailed(error) {
                var msg = 'Save failed: ' + getErrorMessages(error);
                logError(msg, error);
                error.message = msg;
                throw error;
            }
        };

        var primeData = function () {
            return manager.fetchMetadata(manager.dataService);

        };


        var hasChanges = ko.observable(false);

        manager.hasChangesChanged.subscribe(function (eventArgs) {
            hasChanges(eventArgs.hasChanges);
        });

        var datacontext = {
            getPriorityList: getPriorityList,
            getTicketTypeList: getTicketTypeList,
            getCategoryList: getCategoryList,
            getStatusList: getStatusList,
            hasChanges: hasChanges,
            getTicketById: getTicketById,
            primeData: primeData,
            cancelChanges: cancelChanges,
            saveChanges: saveChanges,
            ticketEntityManager: ticketEntityManager
        };

        return datacontext;

        //#region Internal methods  

        function getSettingList(resource, observable, forceRemote) {
            if (!forceRemote) {
                var p = getLocalLookup(resource, true);
                if (p.length > 0) {
                    observable(p);
                    return Q.resolve();
                }
            }

            var query = entityQuery.from(resource);

            return manager.executeQuery(query)
                .then(querySucceded)
                .fail(queryFailed);

            function querySucceded(data) {
                if (observable) {
                    observable(data.results);
                }
                log('Retrieved [' + resource + '] from remote data source',
                    data, true);
            }
        };

        function getLocalLookup(resource) {
            var query = entityQuery.from(resource);
            return manager.executeQueryLocally(query);
        }


        function getErrorMessages(error) {
            var msg = error.message;
            if (msg.match(/validation error/i)) {
                return getValidationMessages(error);
            }
            return msg;
        }

        function getValidationMessages(error) {
            try {
                //foreach entity with a validation error
                return error.entitiesWithErrors.map(function (entity) {
                    // get each validation error
                    return entity.entityAspect.getValidationErrors().map(function (valError) {
                        // return the error message from the validation
                        return valError.errorMessage;
                    }).join('; <br/>');
                }).join('; <br/>');
            }
            catch (e) { }
            return 'validation error';
        }

        function queryFailed(error) {
            var msg = 'Error retreiving data. ' + error.message;
            logError(msg, error);
            throw error;
        }

        function configureBreezeManager() {
            breeze.NamingConvention.camelCase.setAsDefault();
            var mgr = new breeze.EntityManager(config.remoteServiceName);
            model.configureMetadataStore(mgr.metadataStore);
            return mgr;
        }



        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(datacontext), showToast);
        }

        function logError(msg, error) {
            logger.logError(msg, error, system.getModuleId(datacontext), true);
        }
        //#endregion
    });