import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpErrorResponse} from '@angular/common/http'
import { shareReplay, tap, map, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Observable,throwError } from 'rxjs';
import { User } from '../models/models'
import { environment } from 'src/environments/environment';

const apiUrl = environment.apiUrl;

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private httpClient:HttpClient, private router: Router){
  }

  login(username: string, password:string){
      this.httpClient.post(apiUrl+'user/authenticate', {
        username,
        password
      }, {withCredentials:true}).subscribe(
        data=>{
          alert("Successfully logged in!");
          this.router.navigate(['/retailer-data'])
          .then(() => window.location.reload())
        },
        error => {
          if (error.status === 500 || error.status === 500){
            alert("The credential entered are invalid.");
          }
        },
      )
  }

  register(email: string, username: string, password:string){
    return this.httpClient.post(apiUrl+'user', {
    email,
    username,
    password
    }, { observe: 'response'}).pipe(
      shareReplay(),
      tap((res: HttpResponse<any>) => {
        alert("Successfully signed up, please log in!");
        this.navToLogin();
      }),
      catchError((error: HttpErrorResponse): Observable<any> => {
        if (error.status === 400) {
          alert("This E-mail, Username (or both) already exists in our system, try logging in instead!");
        }
        return throwError(error);
      },
    ))
  }

  getAllUsers(){
    return this.httpClient.get<User[]>(apiUrl+'user', {withCredentials:true})
    .pipe(
      map((res) => {
        return <User[]>res;
      }),
    );
  }

  refreshAccessToken(){
    return this.httpClient.post(apiUrl+'user/refresh', {}, {withCredentials:true})
    .pipe(
      map((res) => {
        return res;
      }),
    );
  }

  setAdmin(emailText: string){
    let user = new User();
    user.email = emailText;
    return this.httpClient.post(apiUrl+'user/add_admin', user, {withCredentials:true})
    .pipe(
      map((res) => {
        return res;
      }),
    );
  }

  deleteUser(emailText: string){
    let user = new User();
    user.email = emailText;
    return this.httpClient.post(apiUrl+'user/deleteuser', user, {withCredentials:true})
    .pipe(
      map((res) => {
        return res;
      }),
    );
  }

  signout(){
    let request = this.httpClient.post(apiUrl+'user/logout',{}, {withCredentials:true});

    request.subscribe(()=>{});
  }

  isLoggedIn(){
    return(this.httpClient.post(apiUrl+'user/isauthenticated', {}, {withCredentials:true})
    .toPromise());
  }

  navToTasks(){
    this.router.navigate(['/retailer-data']);
  }

  navToLogin(){
    this.router.navigate(['/login']);
  }
}
