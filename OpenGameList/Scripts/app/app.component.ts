import { Component } from "@angular/core";

@Component({
    selector: "opengamelist",
    template: `
<nav class="navbar navbar-default navbar-fixed-top">
	<div class="container-fluid">
		<input type="checkbox" id="navbar-toggle-cbox"/>
		<div class="navbar-header">
			<label for="navbar-toggle-cbox" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
				<span class="sr-only">Toggle navigation</span>
				<span class="icon-bar"></span>
				<span class="icon-bar"></span>
				<span class="icon-bar"></span>
			</label>
			<a>
				<img alt="logo" src="/img/logo.svg"/>
			</a>
		</div>
		<div class="collapse navbar-collapse" id="navbar">
			<ul class="nav navbar-nav">
				<li><a class="home" [routerLink]="['']">Home</a></li>
				<li><a class="about" [routerLink]="['about']">About</a></li>
				<li><a class="login" [routerLink]="['login']">Login</a></li>
				<li><a class="add" [routerLink]="['item/edit',0]">Add New</a></li>
			</ul>
		</div>
	</div>
</nav>
<h1 class="header">{{title}}</h1>
<div class="main-container">
	<router-outlet></router-outlet>
</div>`
})

export class AppComponent{
    title = "OpenGameList";
}