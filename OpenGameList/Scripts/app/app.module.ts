///<reference path="../../typings/index.d.ts"/>

import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { HttpModule } from "@angular/http";
import "rxjs/Rx";

import { AppComponent } from "./app.component";
import { ItemListComponent } from "./item-list.component";
import { ItemService } from "./item.service";

@NgModule({
    declarations: [
        AppComponent,
        ItemListComponent
    ],
    imports: [BrowserModule, HttpModule],
    providers: [ItemService],
    bootstrap: [AppComponent]
})

export class AppModule { }
