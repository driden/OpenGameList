System.register(["@angular/router", "./home.component", "./about.component", "./login.component", "./page-not-found.component", "./item-detail.component"], function (exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var router_1, home_component_1, about_component_1, login_component_1, page_not_found_component_1, item_detail_component_1, appRoutes, AppRoutingProividers, AppRouting;
    return {
        setters: [
            function (router_1_1) {
                router_1 = router_1_1;
            },
            function (home_component_1_1) {
                home_component_1 = home_component_1_1;
            },
            function (about_component_1_1) {
                about_component_1 = about_component_1_1;
            },
            function (login_component_1_1) {
                login_component_1 = login_component_1_1;
            },
            function (page_not_found_component_1_1) {
                page_not_found_component_1 = page_not_found_component_1_1;
            },
            function (item_detail_component_1_1) {
                item_detail_component_1 = item_detail_component_1_1;
            }
        ],
        execute: function () {
            appRoutes = [
                {
                    path: "",
                    component: home_component_1.HomeComponent
                },
                {
                    path: "home",
                    redirectTo: ""
                },
                {
                    path: "about",
                    component: about_component_1.AboutComponent
                },
                {
                    path: "login",
                    component: login_component_1.LoginComponent
                },
                {
                    path: "item/:id",
                    component: item_detail_component_1.ItemDetailComponent
                },
                {
                    path: '**',
                    component: page_not_found_component_1.PageNotFoundComponent
                }
            ];
            exports_1("AppRoutingProividers", AppRoutingProividers = []);
            exports_1("AppRouting", AppRouting = router_1.RouterModule.forRoot(appRoutes));
        }
    };
});
//# sourceMappingURL=app.routing.js.map