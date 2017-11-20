import { Component } from '@angular/core';
@Component({
    selector: 'my-app',
    template: `<h1>Hi {{a}}</h1>`,
})
export class AppComponent { a = 'Congratulation! you have created your first application using Angular2 with ASP.NET MVC 5'; }