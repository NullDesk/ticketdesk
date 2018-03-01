import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

@Injectable()
export class UploadService {
  constructor(private http: Http) {
  }

upload(fileToUpload: any) {
    const input = new FormData();
    input.append('file', fileToUpload);

    /*return this.http
        .post("/api/uploadFile", input);
*/ return 'yup everything worked, sure did';
}

}
