define(
    [
        'durandal/system',
        'services/model',
        'config',
        'services/logger'
    ],
    function (system, model, config, logger) {
        var entityQuery = breeze.EntityQuery;
        var manager = configureSecurityBreezeManager();
        var orderBy = model.orderBy;
        var entityNames = model.entityNames;

        var primeData = function () {
            log('Priming Security Data', null, true);
            manager = configureSecurityBreezeManager();
            return manager.fetchMetadata(manager.dataService);
        };

        var getTdStaff = function(usersObservable, forceRemote) {
            if (!forceRemote) {
                var p = getUserLookup(entityNames.staffList, true);
                if (p.length > 0) {
                    observable(p);
                    return Q.resolve();
                }
            }

            return getRemoteUserLookup(usersObservable, entityNames.staffList);
        };

        var getTdSubmitters = function(usersObservable, forceRemote) {
            if (!forceRemote) {
                var p = getUserLookup(entityNames.submitterList, true);
                if (p.length > 0) {
                    observable(p);
                    return Q.resolve();
                }
            }
            return getRemoteUserLookup(usersObservable, entityNames.submitterList);
        };

        var securitycontext = {
            primeData: primeData,
            getTdSubmitters: getTdSubmitters,
            getTdStaff: getTdStaff
        };

        return securitycontext;



        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(securitycontext), showToast);
        }
        

        function getUserLookup(resource) {
            var query = entityQuery.from(resource).orderBy(orderBy.tdUser);
            return manager.executeQueryLocally(query);
        }
        
        function getRemoteUserLookup(usersObservable, resource) {
            var query = entityQuery.from(resource).orderBy(orderBy.tdUser);

            return manager.executeQuery(query)
                .then(querySucceded)
                .fail(queryFailed);

            function querySucceded(data) {
                if (usersObservable) {
                    usersObservable(data.results);
                }
                log('Retrieved [' + resource + '] from remote data source',
                    data, true);
            }
        };

        function queryFailed(error) {
            var msg = 'Error retreiving data. ' + error.message;
            logError(msg, error);
            throw error;
        }

        function configureSecurityBreezeManager() {
            breeze.NamingConvention.camelCase.setAsDefault();
            var mgr = new breeze.EntityManager(config.remoteSecurityServiceName);

            if (!mgr.metadataStore.hasMetadataFor(config.remoteSecurityServiceName)) {
                model.configureSecurityMetadataStore(mgr.metadataStore);
            }

            return mgr;
        }

    }
);