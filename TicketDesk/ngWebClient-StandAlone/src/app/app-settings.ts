export const serviceBaseURL = 'http://localhost:50067/api';
export const activeDirectoryUserURL = serviceBaseURL + '/users/';
export const ticketDetailsURL = serviceBaseURL + '/ticket/';
export const ticketEventsURL = ticketDetailsURL + 'events/';
export const actionURLs = {
  1 : serviceBaseURL + '/actions/add-comment',
  2 : serviceBaseURL + '/actions/supply-more-info',
  4 : serviceBaseURL + '/actions/cancel-more-info',
  8 : serviceBaseURL + '/actions/request-more-info',
  16 : serviceBaseURL + '/actions/take-over',
  32 : serviceBaseURL + '/actions/resolve',
  64 : serviceBaseURL + '/actions/assign',
  128 : serviceBaseURL + '/actions/reassign',
  256 : serviceBaseURL + '/actions/pass',
  512 : serviceBaseURL + '/actions/close',
  1024 : serviceBaseURL + '/actions/reopen',
  2048 : serviceBaseURL + '/actions/give-up',
  4096 : serviceBaseURL + '/actions/force-close',
  8192 : serviceBaseURL + '/actions/modify-attachments',
  16384 : serviceBaseURL + '/actions/edit-ticket-info',
  32768 : serviceBaseURL + '/actions/create',
  65536 : serviceBaseURL + '/actions/create-on-behalf-of/',
};

export const getValidActionsURL = serviceBaseURL + '/actions/activity-buttons/';
export const prioritiesURL = serviceBaseURL + '/ticket/priorities';
export const ticketTypesURL = serviceBaseURL + '/ticket/types';
export const categoriesURL = serviceBaseURL + '/ticket/categories';
export const resetTicketsFilterAndSort = serviceBaseURL + '/tickets/reset-user-lists';
export const getTicketList = serviceBaseURL + '/tickets/pageList';
export const getSortedTicketList = serviceBaseURL + '/tickets/sortList';
export const getFilteredTicketList = serviceBaseURL + '/tickets/filterList';
export const getUserPermissions = serviceBaseURL + '/users/permissions';
export const searchTerm = serviceBaseURL + '/search/';
