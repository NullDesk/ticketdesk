import { AdContactService } from './../services/ad-contact.service';
import { UserDetails } from 'app/models/user-details';
import { Component, OnInit, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NUMBER_TYPE } from '@angular/compiler/src/output/output_ast';


@Component({
  selector: 'app-contact-info',
  templateUrl: './contact-info.component.html',
  styleUrls: ['./contact-info.component.css']
})
export class ContactInfoComponent implements OnInit {
  @Input()
  ownerId: string;
  user: UserDetails;

  constructor(private router: Router,
    private contactService: AdContactService,
    private activatedRoute: ActivatedRoute) {
    // this.activatedRoute.params.subscribe(params => {
    //   if (params[OWNER_ID_PARAM] === ''
    //     || isNaN(Number(params[OWNER_ID_PARAM]))) {
    //     this.router.navigate(['/NaNTickedId'])
    //     return
    //   }
    //   this.ownerId = _ownerId
    // });
  }

  ngOnInit() {
    this.contactService.getContactCardInfo(this.ownerId)
      .subscribe(res => {
        this.user = res;
      });
  }

}
