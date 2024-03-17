import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { map } from 'rxjs/operators';
import { ScrapeRequest } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class ScrapeRequestService {
  constructor(private httpClient:HttpClient){
   }

   requestService(request: ScrapeRequest){
    console.log(request)
    this.httpClient.post('http://localhost:33272/api/scraper-service', request, {withCredentials:true}).subscribe(
      data=>{
        alert("Successfully sent scrape request!");
      });
   }
}
