///<reference path="../../typings/index.d.ts"/>
System.register(["@angular/core", "@angular/platform-browser", "@angular/http", "@angular/forms", "@angular/router", "rxjs/Rx", "./about.component", "./app.component", "./home.component", "./item-list.component", "./item.service", "./item-detail.component", "./login.component", "./page-not-found.component", "./app.routing"], function (exports_1, context_1) {
    "use strict";
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __moduleName = context_1 && context_1.id;
    var core_1, platform_browser_1, http_1, forms_1, router_1, about_component_1, app_component_1, home_component_1, item_list_component_1, item_service_1, item_detail_component_1, login_component_1, page_not_found_component_1, app_routing_1, AppModule;
    return {
        setters: [
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (platform_browser_1_1) {
                platform_browser_1 = platform_browser_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (forms_1_1) {
                forms_1 = forms_1_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
            },
            function (_1) {
            },
            function (about_component_1_1) {
                about_component_1 = about_component_1_1;
            },
            function (app_component_1_1) {
                app_component_1 = app_component_1_1;
            },
            function (home_component_1_1) {
                home_component_1 = home_component_1_1;
            },
            function (item_list_component_1_1) {
                item_list_component_1 = item_list_component_1_1;
            },
            function (item_service_1_1) {
                item_service_1 = item_service_1_1;
            },
            function (item_detail_component_1_1) {
                item_detail_component_1 = item_detail_component_1_1;
            },
            function (login_component_1_1) {
                login_component_1 = login_component_1_1;
            },
            function (page_not_found_component_1_1) {
                page_not_found_component_1 = page_not_found_component_1_1;
            },
            function (app_routing_1_1) {
                app_routing_1 = app_routing_1_1;
            }
        ],
        execute: function () {///<reference path="../../typings/index.d.ts"/>
            AppModule = class AppModule {
            };
            AppModule = __decorate([
                core_1.NgModule({
                    declarations: [
                        about_component_1.AboutComponent,
                        app_component_1.AppComponent,
                        home_component_1.HomeComponent,
                        item_list_component_1.ItemListComponent,
                        item_detail_component_1.ItemDetailComponent,
                        login_component_1.LoginComponent,
                        page_not_found_component_1.PageNotFoundComponent
                    ],
                    imports: [
                        platform_browser_1.BrowserModule,
                        http_1.HttpModule,
                        forms_1.FormsModule,
                        router_1.RouterModule,
                        app_routing_1.AppRouting
                    ],
                    providers: [item_service_1.ItemService],
                    bootstrap: [app_component_1.AppComponent]
                })
            ], AppModule);
            exports_1("AppModule", AppModule);
        }
    };
});
//# sourceMappingURL=app.module.js.map