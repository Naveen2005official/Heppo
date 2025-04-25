import { CanActivateFn } from '@angular/router';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);
  if(accountService.currentUser())
  {
    return true;
  }
  else
  {
    toastr.error('You shall not pass!');
    return false;
  }
  return true;
};
