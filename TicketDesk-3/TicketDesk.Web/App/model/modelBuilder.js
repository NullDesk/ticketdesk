/** 
 * @module This module has the responsability of creating breeze entities
 *         Add here all your initializers, constructors, ...etc
 * @requires appsecurity
 * @requires utils
 */
define(['services/appsecurity', 'services/utils'],
    function (appsecurity, utils) {
        var foreignKeyInvalidValue = 0;

        var self = {
            extendMetadata: extendMetadata
        };

        return self;

        /**
         * Extend entities
         * param {BreezeManagerMetadataStore} metadataStore - The breeze metadata store
         */
        function extendMetadata(metadataStore) {
            
            addSimpleSettingTypes(metadataStore);

           
            extendTicket(metadataStore);
        }

        function addSimpleSettingTypes(metadataStore) {

            var simpleSettingDataProperties = {
                name: { dataType: breeze.DataType.String, isNullable: false, isPartOfKey: false },
                value: { dataType: breeze.DataType.String, isNullable: false, isPartOfKey: true },
            };

            metadataStore.addEntityType({
                shortName: 'PrioritySetting',
                namespace: 'TicketDesk.Domain.Model',
                dataProperties: simpleSettingDataProperties
            });
            metadataStore.addEntityType({
                shortName: 'CategorySetting',
                namespace: 'TicketDesk.Domain.Model',
                dataProperties: simpleSettingDataProperties
            });
            metadataStore.addEntityType({
                shortName: 'TicketTypeSetting',
                namespace: 'TicketDesk.Domain.Model',
                dataProperties: simpleSettingDataProperties
            });

            metadataStore.addEntityType({
                shortName: 'TicketStatusSetting',
                namespace: 'TicketDesk.Domain.Model',
                dataProperties: simpleSettingDataProperties
            });

            metadataStore.setEntityTypeForResourceName('PriorityList', 'PrioritySetting');
            metadataStore.setEntityTypeForResourceName('TicketTypeList', 'CategorySetting');
            metadataStore.setEntityTypeForResourceName('CategoryList', 'TicketTypeSetting');
            metadataStore.setEntityTypeForResourceName('StatusList', 'TicketStatusSetting');
            
            metadataStore.setEntityTypeForResourceName('TagSuggestionList', 'TicketTag');
        }

        /**
         * Extend tickets
         * param {BreezeManagerMetadataStore} metadataStore - The breeze metadata store
         */
        function extendTicket(metadataStore) {
            var ticketCtor = function () {
                this.isPartial = false;
            };

            var ticketInitializer = function (ticket) {
                ticket.tagListFormatted = ko.computed({
                    read: function () {
                        var text = ticket.tagList();
                        return text ? text.replace(/\,/g, ', ') : text;
                    },
                    write: function (value) {
                        ticket.tagList(value.replace(/\, /g, ','));
                    }
                });
                ticket.createdCombined = ko.computed({
                    read: function () {
                        return $.i18n.t("appmodeltext:ticketCreatedCombined", {
                            date: moment(ticket.createdDate()).format('lll'),
                            user: ticket.createdBy()
                        });
                    }
                });

                ticket.lastUpdatedCombined = ko.computed({
                    read: function () {
                        return $.i18n.t("appmodeltext:ticketLastUpdatedCombined", {
                            date: moment(ticket.lastUpdateDate()).format('lll'),
                            user: ticket.lastUpdateBy()
                        });
                    }
                });
            };
            metadataStore.registerEntityTypeCtor('Ticket', ticketCtor, ticketInitializer);
            cleanupValidators(metadataStore);
        }

        function cleanupValidators(metadataStore) {
            //not needed by the client, and gets in the way of new ticket submission; 
            //  these are all set automatically on the server
            removeValidator(metadataStore, "Ticket", "createdBy", "required");
            removeValidator(metadataStore, "Ticket", "currentStatusSetBy", "required");
            removeValidator(metadataStore, "Ticket", "lastUpdateBy", "required");
        }

        function removeValidator(metadataStore, entityName, propertyName, validatorName) {
            var validators = metadataStore.getEntityType(entityName).getProperty(propertyName).validators;
            var theValidator = $.grep(validators, function (e) { return e.name === validatorName; });
            var idxOf = $.inArray(theValidator[0], validators);
            validators.splice(idxOf, 1);
        }
    });