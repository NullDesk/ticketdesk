export interface UserDetails {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  groups: string[];
}

export const userPermissions = {
  admin: 'TD_Admin',
  user: 'TD_User',
  resolver: 'TD_Resolver'
};
