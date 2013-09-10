define(['config', 'durandal/system', 'services/logger'],
    function (config, system, logger) {

        var orderBy = {
            ticket: 'lastUpdateDate desc, assignedTo',
            tdUser: 'userName'
        };

        var entityNames = {
            ticket: 'Ticket',
            priorityList: 'PriorityList',
            categoryList: 'CategoryList',
            ticketTypeList: 'TicketTypeList',
            statusList: 'StatusList',
            submitterList: 'SubmitterList',
            staffList: 'StaffList'
        };

        var model = {
            configureSecurityMetadataStore: configureSecurityMetadataStore,
            configureMetadataStore: configureMetadataStore,
            entityNames: entityNames,
            orderBy: orderBy
        };

        return model;


        //#region Internal Methods
        
        function configureSecurityMetadataStore(metadataStore) {
            //this is a sort-of-hack: see :
            //  http://stackoverflow.com/questions/16727432/
            metadataStore.setEntityTypeForResourceName("TdUser", "TdUser:#TicketDesk.Domain.Identity");
            metadataStore.setEntityTypeForResourceName("SubmitterList", "TdUser:#TicketDesk.Domain.Identity");
            metadataStore.setEntityTypeForResourceName("StaffList", "TdUser:#TicketDesk.Domain.Identity");
            
            
        }

        function configureMetadataStore(metadataStore) {
            addSimpleSettingTypes(metadataStore);
            metadataStore.registerEntityTypeCtor('Ticket', function () { this.isPartial = false; }, ticketInitializer);
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