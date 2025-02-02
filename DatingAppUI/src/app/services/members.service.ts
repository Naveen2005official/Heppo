import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../models/photo';
import { PaginationResult } from '../models/pagination';
import { UserParams } from '../models/user-params';
import { AccountService } from './account.service';
import { setPaginatedResponse, setPaginationHeaders } from './pagination-helper.service';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient)
  private accountService = inject(AccountService)
  baseUrl = environment.apiUrl
  paginatedResult = signal<PaginationResult<Member[]> | null>(null);
  memberCache = new Map();
  user = this.accountService.currentUser();
  userParams = signal<UserParams>(new UserParams(this.user));

  resetUserParams()
  {
    this.userParams.set(new UserParams(this.user));
  }

  getMembers()
  {
    const response = this.memberCache.get(Object.values(this.userParams()).join('-'));
    if(response) return setPaginatedResponse(response, this.paginatedResult);
    let params = setPaginationHeaders(this.userParams().pageNumber, this.userParams().pageSize);

    params = params.append('minAge', this.userParams().minAge);
    params = params.append('maxAge', this.userParams().maxAge);
    params = params.append('gender', this.userParams().gender);
    params = params.append('orderBy', this.userParams().orderBy);
    return this.http.get<Member[]>(this.baseUrl + 'User', {observe : 'response', params}).subscribe({
      next: response => {
        setPaginatedResponse(response, this.paginatedResult);
        this.memberCache.set(Object.values(this.userParams()).join('-'), response);
      }
    })
  }

  getMember(username : string)
  {
    const member: Member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.body),[])
      .find((m: Member) => m.username === username);
    console.log(member);
    
      if(member) return of(member);
    return this.http.get<Member>(this.baseUrl + 'User/' + username);
  }

  updateMember(member: Member)
  {
    return this.http.put(this.baseUrl + 'User',member).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => m.username === member.username ? member : m))
      // })
    )
  }

  setMainPhoto(photo: Photo) {
    return this.http.put(this.baseUrl + 'User/set-main-photo/' + photo.id, {}).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if (m.photos.includes(photo)) {
      //       m.photoUrl = photo.url
      //     }
      //     return m;
      //   }))
      // })
    )
  }

  deletePhoto(photo: Photo) {
    return this.http.delete(this.baseUrl + 'User/delete-photo/' + photo.id).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if (m.photos.includes(photo)) {
      //       m.photos = m.photos.filter(x => x.id !== photo.id)
      //     }
      //     return m
      //   }))
      // })
    )
  }
}
