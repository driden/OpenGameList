import { ModuleWithProviders } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { HomeComponent } from "./home.component";
import { AboutComponent } from "./about.component";
import { LoginComponent } from "./login.component";
import { PageNotFoundComponent } from "./page-not-found.component";
import { ItemDetailComponent } from "./item-detail.component";

const appRoutes: Routes = [
    {
        path: "",
        component: HomeComponent
    },
    {
        path: "home",
        redirectTo:""
    },
    {
        path: "about",
        component:AboutComponent
    },
    {
        path: "login",
        component : LoginComponent
    },
    {
        path: "item/:id",
        component: ItemDetailComponent
    },
    {
        path: '**',
        component: PageNotFoundComponent
    }    
];

export const AppRoutingProividers: any[] = [];

export const AppRouting: ModuleWithProviders = RouterModule.forRoot(appRoutes);