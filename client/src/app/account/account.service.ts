import { Injectable } from '@angular/core';
import { ReplaySubject, map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../shared/models/user';
import { HttpClient, HttpHandler, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({                   //188
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User | null>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  loadCurrentUser(token: string | null) {        //store the token so it remembers the user 194
    if (token === null) {
      this.currentUserSource.next(null);      //205
      return of(null);
    }
    
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);  //194
  
    return this.http.get<User>(this.baseUrl + 'account', {headers}).pipe(   //194
      map(user => {
        if (user){
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
          return user;
        } else {
          return null;
        }
        
      })
    )
  }

  login(values: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', values).pipe(      //user object has a token and we want to keep that if user closes the app
      map(user => {
        localStorage.setItem('token', user.token);
        this.currentUserSource.next(user);
      })
    )  
  }

  register(values: any) {         //recive values from a form
    return this.http.post<User>(this.baseUrl + 'account/register', values).pipe(     
      map(user => {
        localStorage.setItem('token', user.token);
        this.currentUserSource.next(user);
      })
    )
  }   
  
  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    return this.http.get<boolean>(this.baseUrl + 'account/emailExists?email=' + email);
  }
}
