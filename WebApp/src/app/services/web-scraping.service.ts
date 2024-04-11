import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { environment } from 'src/environments/environment';
import { ScrapeRequest } from '../models/models';

const apiUrl = environment.apiUrl;

@Injectable({
  providedIn: 'root'
})
export class ScrapeRequestService {
  constructor(private httpClient:HttpClient){
   }

   requestService(request: ScrapeRequest){
    console.log(request)
    this.httpClient.post(apiUrl+'scraper-service', request, {withCredentials:true}).subscribe(
      data=>{
        alert("Successfully sent scrape request!");
      });
   }
}
