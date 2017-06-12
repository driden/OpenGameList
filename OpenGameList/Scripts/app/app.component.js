System.register(["@angular/core"], function (exports_1, context_1) {
    "use strict";
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __moduleName = context_1 && context_1.id;
    var core_1, AppComponent;
    return {
        setters: [
            function (core_1_1) {
                core_1 = core_1_1;
            }
        ],
        execute: function () {
            AppComponent = class AppComponent {
                constructor() {
                    this.title = "OpenGameList";
                }
            };
            AppComponent = __decorate([
                core_1.Component({
                    selector: "opengamelist",
                    template: `<h1>{{title}}</h1>
                <div class="menu">
                    <a class="home" [routerLink]="['']">Home</a>
                    | <a class="about" [routerLink]="['about']">About</a>
                    | <a class="login" [routerLink]="['login']">Login</a>
                    | <a class="add" [routerLink]="['item/edit', 0]">Add New</a>
                </div>
                <router-outlet></router-outlet>
                `
                })
            ], AppComponent);
            exports_1("AppComponent", AppComponent);
        }
    };
});
//# sourceMappingURL=app.component.js.map