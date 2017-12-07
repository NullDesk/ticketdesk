import { Component, OnInit, ViewChild } from '@angular/core';
import {UploadService} from '../services/upload.service';
@Component({
  selector: 'app-attach-file',
  templateUrl: './attach-file.component.html',
  styleUrls: ['./attach-file.component.css'],
  providers: [UploadService]
})
export class AttachFileComponent implements OnInit {

  constructor(private uploadService: UploadService) { }

  ngOnInit() {
  };

	@ViewChild("fileInput") fileInput;

public hasAFile(): boolean {
    let fi = this.fileInput.nativeElement;
  return (fi.files && fi.files[0])
};

addFile(): void {
	let fi = this.fileInput.nativeElement;
    if (fi.files && fi.files[0]) {
        let fileToUpload = fi.files[0];
        this.uploadService
            .upload(fileToUpload);
            /* .subscribe(res => {
                console.log(res);
            });*/
    };
};
}
