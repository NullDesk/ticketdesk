/** 
 * @module Derived repository for the Ticket entity
*/

define(['services/repository', 'services/breeze.partial-entities'], function (repository, partialMapper) {
    /**
         * Repository ctor
         * @constructor
        */
    var TicketRepository = (function () {
        var ticketrepository = function (entityManagerProvider, entityTypeName, resourceName) {
            repository.getCtor.call(this, entityManagerProvider, entityTypeName, resourceName);

            this.openTicketRowCount = ko.observable(0);
            

            this.getOpenTicketPagedList = function (ticketsObservable, forPageIndex, forceRemote) {
                var predicate = breeze.Predicate.create('ticketStatus', '!=', 'Closed');
                if (forceRemote || this.openTicketRowCount() < 1) {
                    return fetchRemoteTicketPartials(predicate).then(function () {
                        fetchLocalTicketPartials(predicate, ticketsObservable, forPageIndex);
                    });
                } else {
                    return fetchLocalTicketPartials(predicate, ticketsObservable, forPageIndex);
                }
            };
            

            var baseQuery = breeze.EntityQuery.from(resourceName)
                .orderBy('lastUpdateDate desc, assignedTo');
                 
            function fetchLocalTicketPartials(predicate, ticketsObservable, forPageIndex) {
                var query = baseQuery
                    .where(predicate)
                    .skip(forPageIndex * 5)
                    .take(5)
                    .using(breeze.FetchStrategy.FromLocalCache);
                return executeQuery(query).then(function (data) {
                    if (data.results.length > 0) {
                        ticketsObservable(data.results);
                    }
                });
            }

            var rowCount = this.openTicketRowCount;

            function fetchRemoteTicketPartials(predicate) {
                var query = baseQuery
                    .where(predicate)
                    .select('ticketId, title, ticketType, owner, assignedTo, ticketStatus, category, priority, createdBy, createdDate, lastUpdateBy, lastUpdateDate')
                    .using(breeze.FetchStrategy.FromServer)
                    .inlineCount(true);

                return executeQuery(query).then(querySucceeded);

                function querySucceeded(data) {
                    rowCount(data.inlineCount);
                    partialMapper.mapDtosToEntities(entityManagerProvider.manager(), data.results, entityTypeName, 'ticketId');
                    //log('Retrieved [Tickets] from remote data source', data, true);
                }
            }

            function executeQuery(query) {
                return entityManagerProvider.manager()
                    .executeQuery(query);
                //.then(function (data) { return data.results; });
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