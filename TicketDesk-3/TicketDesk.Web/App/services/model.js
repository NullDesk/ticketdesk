define(['config', 'durandal/system', 'services/logger'],
    function (config, system, logger) {

        var orderBy = {
            ticket: 'lastUpdateDate desc, assignedTo'
        };

        var entityNames = {
            ticket: 'Ticket',
            priorityList: 'PriorityList',
            categoryList: 'CategoryList',
            ticketTypeList: 'TicketTypeList',
            statusList: 'StatusList'
        };



        var model = {
            configureMetadataStore: configureMetadataStore,
            entityNames: entityNames,
            orderBy: orderBy
        };

        return model;


        //#region Internal Methods
        function configureMetadataStore(metadataStore) {
            addPrioritySettingType(metadataStore);

            metadataStore.registerEntityTypeCtor('Ticket', function () { this.isPartial = false; }, ticketInitializer);
        }

        function addPrioritySettingType(metadataStore) {
            metadataStore.addEntityType({
                shortName: 'PrioritySetting',
                namespace: 'TicketDesk.Domain.Model',
                dataProperties: {
                    value: { dataType: breeze.DataType.String, isNullable: false, isPartOfKey: true },
                }
            });
            metadataStore.addEntityType({
                shortName: 'CategorySetting',
                namespace: 'TicketDesk.Domain.Model',
                dataProperties: {
                    value: { dataType: breeze.DataType.String, isNullable: false, isPartOfKey: true },
                }
            });
            metadataStore.addEntityType({
                shortName: 'TicketTypeSetting',
                namespace: 'TicketDesk.Domain.Model',
                dataProperties: {
                    value: { dataType: breeze.DataType.String, isNullable: false, isPartOfKey: true },
                }
            });

            metadataStore.addEntityType({
                shortName: 'TicketStatusSetting',
                namespace: 'TicketDesk.Domain.Model',
                dataProperties: {
                    value: { dataType: breeze.DataType.String, isNullable: false, isPartOfKey: true },
                }
            });

            metadataStore.setEntityTypeForResourceName('PriorityList', 'PrioritySetting');
            metadataStore.setEntityTypeForResourceName('TicketTypeList', 'CategorySetting');
            metadataStore.setEntityTypeForResourceName('CategoryList', 'TicketTypeSetting');
            metadataStore.setEntityTypeForResourceName('StatusList', 'TicketStatusSetting');

        }

        function ticketInitializer(ticket) {
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
        }


        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(model), showToast);
        }
        //#endregion
    });