///<reference path="../../typings/index.d.ts"/>

import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { HttpModule } from "@angular/http";
import "rxjs/Rx";

import { AppComponent } from "./app.component";

@NgModule({
    declarations: [AppComponent],
    imports: [BrowserModule, HttpModule],
    providers: [],
    bootstrap: [AppComponent]
})

export class AppModule { }
