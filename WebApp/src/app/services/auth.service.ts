import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpErrorResponse} from '@angular/common/http'
import { shareReplay, tap, map, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Observable,throwError } from 'rxjs';
import { User } from '../models/models'


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private httpClient:HttpClient, private router: Router){
    console.log("Auth online")
  }

  login(username: string, password:string){
      this.httpClient.post('http://localhost:33272/api/user/authenticate', {
        username,
        password
      }, {withCredentials:true}).subscribe(
        data=>{
          alert("Successfully logged in!");
          this.router.navigate(['/sensor-data'])
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
    return this.httpClient.post('http://localhost:33272/api/user', {
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
    return this.httpClient.get<User[]>('http://localhost:33272/api/user', {withCredentials:true})
    .pipe(
      map((res) => {
        return <User[]>res;
      }),
    );
  }

  refreshAccessToken(){
    return this.httpClient.post('http://localhost:33272/api/user/refresh', {}, {withCredentials:true})
    .pipe(
      map((res) => {
        return res;
      }),
    );
  }

  setAdmin(emailText: string){
    let user = new User();
    user.email = emailText;
    return this.httpClient.post('http://localhost:33272/api/user/add_admin', user, {withCredentials:true})
    .pipe(
      map((res) => {
        return res;
      }),
    );
  }

  deleteUser(emailText: string){
    let user = new User();
    user.email = emailText;
    return this.httpClient.post('http://localhost:33272/api/user/deleteuser', user, {withCredentials:true})
    .pipe(
      map((res) => {
        return res;
      }),
    );
  }

  signout(){
    let request = this.httpClient.post('http://localhost:33272/api/user/logout',{}, {withCredentials:true});

    request.subscribe(()=>{});
  }

  isLoggedIn(){
    return(this.httpClient.post('http://localhost:33272/api/user/isauthenticated', {}, {withCredentials:true})
    .toPromise());
  }

  navToTasks(){
    this.router.navigate(['/sensor-data']);
  }

  navToLogin(){
    this.router.navigate(['/login']);
  }
}
