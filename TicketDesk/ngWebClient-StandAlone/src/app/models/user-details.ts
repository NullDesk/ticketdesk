export interface UserDetails {
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string;
  userId: string;
}

export const userPermissions = {
  admin: 'TD_Admin',
  user: 'TD_User',
  resolver: 'TD_Resolver'
};
