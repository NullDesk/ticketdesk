define(
    function () {
        var defaultExtension = { isPartial: true };
        var mapper = {
            mapDtosToEntities: mapDtosToEntities
        };

        return mapper;

        /**
        Map an array of DTO's for a type of entity 
        to an array of Breeze entities that are managed by 
        the entity manager and are observables.
            @method mapDtosToEntities
            @param manager {Object} The breeze manager instance
            @param dtos {Object|Array of Object} The objects to map to entities
            @param entityName {String} The name of the entity
            @param keyName {String} The name of the entity key property
            @param mapping {Object=defaultMapping} Object containing the default mapping for the entity
            @returns {Object} breeze entity
        **/
        function mapDtosToEntities(manager, dtos, entityName, keyName, extendWith) {
            return dtos.map(dtoToEntityMapper);

            function dtoToEntityMapper(dto) {
                var keyValue = dto[keyName];
                var entity = manager.getEntityByKey(entityName, keyValue);
                if (!entity) {
                    // We don't have it, so create it as a partial
                    extendWith = $.extend({ }, extendWith || defaultExtension);
                    extendWith[keyName] = keyValue;
                    entity = manager.createEntity(entityName, extendWith);
                }
                mapToEntity(entity, dto);
                entity.entityAspect.setUnchanged();
                return entity;
            }

            function mapToEntity(entity, dto) {
                // entity is an object with observables
                // dto is from json
                for (var prop in dto) {
                    if (dto.hasOwnProperty(prop)) {
                        entity[prop](dto[prop]);
                    }
                }
                return entity;
            }
        }
    });