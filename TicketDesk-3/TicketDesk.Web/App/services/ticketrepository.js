/** 
 * @module Derived repository for the Ticket entity
*/

define(['durandal/system','services/repository', 'services/logger'], function (system, repository, logger) {
    /**
         * Repository ctor
         * @constructor
        */
    var TicketRepository = (function () {
        var ticketrepository = function (entityManagerProvider, entityTypeName, resourceName) {
            repository.getCtor.call(this, entityManagerProvider, entityTypeName, resourceName);
            var self = this;

            var pageSize = 5;

            var getBaseQuery = function () {
                return breeze.EntityQuery.from(resourceName)
                    .orderBy('lastUpdateDate desc, assignedTo');
            };
            var getBasePartialQuery = function () {
                return getBaseQuery()
                    .select('ticketId, title, ticketType, owner, assignedTo, ticketStatus, category, priority, createdBy, createdDate, lastUpdateBy, lastUpdateDate');
            };
            
            self.openTicketRowCount = ko.observable(0);

            self.getOpenTicketPagedList = function (ticketsObservable, forPageIndex, forceRemote) {
                var predicate = breeze.Predicate.create('ticketStatus', '!=', 'Closed');
                if (forceRemote || this.openTicketRowCount() < 1) {
                    return fetchRemoteTicketPartials(predicate).then(function () {
                        fetchLocalTickets(predicate, ticketsObservable, forPageIndex);
                    });
                } else {
                    return fetchLocalTickets(predicate, ticketsObservable, forPageIndex);
                }
            };

            function fetchLocalTickets(predicate, ticketsObservable, forPageIndex) {
                var query = getBaseQuery()
                    .where(predicate)
                    .skip(forPageIndex * pageSize)
                    .take(pageSize);
                var data = self.executeCacheQuery(query);
                if (data.length > 0) {
                    ticketsObservable(data);
                    logger.log('Retrieved [Tickets] from local cache', data, 'TicketRepository', false);
                }
            }

            function fetchRemoteTicketPartials(predicate) {
                var query = getBasePartialQuery()
                    .where(predicate)
                    .toType(entityTypeName)
                    .using(breeze.FetchStrategy.FromServer)
                    .inlineCount(true);

                return self.executeQuery(query).then(querySucceeded);

                function querySucceeded(data) {
                    self.openTicketRowCount(data.inlineCount);
                    logger.log('Retrieved [Tickets] from remote data source', data, 'TicketRepository', false);
                }
            }
        };

        ticketrepository.prototype = repository.create();
        return ticketrepository;
    })();

    return {
        create: create
    };

    /**
     * Create a new Repository
     * @method
     * @param {EntityManagerProvider} entityManagerProvider
     * @param {string} entityTypeName
     * @param {string} resourceName
     * @param {FetchStrategy} fetchStrategy
     * @return {Repository}
    */
    function create(entityManagerProvider, entityTypeName, resourceName) {
        return new TicketRepository(entityManagerProvider, entityTypeName, resourceName);
    }
});