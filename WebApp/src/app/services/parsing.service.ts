import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { environment } from 'src/environments/environment';
import * as Parser from 'papaparse';

const apiUrl = environment.apiUrl;

@Injectable({
    providedIn: 'root'
  })
  export class ParsingService {
    constructor(private httpClient:HttpClient){
     }

     private jsonData:any[];
  
     parseFile(file: any){
        
        if(file.type == "application/json"){
            const reader = new FileReader();
            reader.readAsText(file, 'UTF-8');

            reader.onload = () => {
                try {
                this.jsonData = JSON.parse(reader.result as string);
                } catch (e) {
                alert('Invalid JSON file!');
                }
            };
        }

        if(file.type !== "text/csv"){
            return;
        }

        Parser.parse(file, {
            complete: (result) => {
                this.jsonData = this.csvJSON(result.data);
            },
            header: true
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

        this.httpClient.post(apiUrl+'shopitems/itemlist-upload', this.jsonData, {withCredentials:true}).subscribe(
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
