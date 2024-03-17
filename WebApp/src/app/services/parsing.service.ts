import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import * as Parser from 'papaparse';

@Injectable({
    providedIn: 'root'
  })
  export class ParsingService {
    constructor(private httpClient:HttpClient){
     }

     private jsonData:any[];
  
     parseFile(file: any){

        if(file.type == "application/json"){
            this.jsonData = JSON.parse(file);
        }

        if(file.type !== "text/csv"){
            return;
        }

        Parser.parse(file, {
            complete: (result) => {
                console.log('Parsed: ', result);
                this.jsonData = this.csvJSON(result.data);
            },
            header: true // Set to true if the first row of CSV are headers
        });
     }

     csvJSON(csv: any[]) {
        const json = [];
        csv.forEach(row => {
            json.push(row);
        });
        return json;
    }

    sendData(){
        if(this.jsonData.length === 0){
            alert("No file selected");
            return;
        }

        this.httpClient.post('http://localhost:33272/api/shopitems/itemlist-upload', this.jsonData, {withCredentials:true}).subscribe(
          data=>{
            alert("Successfully uploaded the files!");
        },
        error => {
          if (error.status === 500){
            alert("There are errors with the file content. Please upload a valid file!");
          }
        });

        this.jsonData = [];
    }
}
  