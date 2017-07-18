import { Component, OnInit } from "@angular/core"
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms"
import { Router } from "@angular/router"
import { AuthService } from "./auth.service"
import { User } from "./user"

@Component({
	selector: "user-edit",
	template: `
<div class="user-container">
	<form class="form-user" [formGroup]="userForm" (submit)="onSubmit()">
		<h2 class="form-user-heading">{{title}}</h2>
		<div class="form-group">
			<input fromControlName="username" type="text" class ="form-control" placeholder="Choose a Username" autofocus/>
			<span class="validator-label valid" *ngIf="this.userForm.controls.username.valid">
				<span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
				valid!
			</span>
			<span class="validator-label invalid" *ngIf="!this.userForm.controls.username.valid && !this.userForm.controls.username.pristine">
				<span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
				invalid
			</span>
		</div>
		<div class="form-group">
			<input formControlName="email" type="text" class="form-control" placeholder="Type your e-mail address"/>
			<span class="validator-label valid" *ngIf="this.userForm.controls.email.valid">
				<span class="glyphicon-glyphicon-ok" aria-hidden="true"></span>
				valid!				
			</span>
			<span	class="validator-label invalid" *ngIf="!this.userForm.controls.email.valid && !this.userForm.controls.email.pristine">
				<span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
				invalid
			</span>
		</div>
		<div class="form-group">
			<input formControlName="password" type="password" class="form-control" placeholder="Choose a Password"/>
			<span class="validator-label valid" *ngIf="this.userForm.controls.password.valid && !this.userForm.controls.password.pristine">
				<span class="glyphicon-glyphicon-ok" aria-hidden="true"></span>
				valid!
			</span>
			<span class="validator-label invalid" *ngIf="!this.userForm.controls.password.valid && !this.userForm.controls.password.pristine">
				<span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
				invalid
			</span>
		</div>
		<div class="form-group">
			<input formControlName="passwordConfirm" type="password" class="form-control" placeholder="Confirm your Password"/>
			<span class="validator-label valid" *ngIf="this.userForm.controls.passwordConfirm.valid && !this.userForm.controls.password.pristine && !this.userForm.hasError('compareFailed')">
				<span class="glyphicon-glyphicon-ok" aria-hidden="true"></span>
				valid!
			</span>
			<span class="validator-label invalid" *ngIf="(!this.userForm.controls.passwordConfirm.valid && !this.userForm.controls.passwordConfirm.pristine) || this.userForm.hasError('compareFailed')">
				<span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
				invalid
			</span>
		</div>
		<div class="form-group">
			<input formControlName="displayName" type="text" class="form-control" placeholder="Choose a Display Name" />
		</div>
		<div class="form-group">
			<input type="submit" class="btn btn-primary btn-block" [disabled]="!userForm.valid" value="Register"/>
		</div>
	</form>
</div>`
})

export class UserEditComponent {
	title = "New User Registration"
	userForm: FormGroup = null
	erroMessage = null

	constructor(
		private fb: FormBuilder,
		private router: Router,
		private authService: AuthService
	) {
		if (this.authService.isLoggedIn()) {
			this.router.navigate([""])
		}
	}

	ngOnInit() {
		this.userForm = this.fb.group({
			username: ["", [
				Validators.required,
				Validators.pattern("[a-zA-Z0-9]+")
			]],
			email: ["", [
				Validators.required,
				Validators.pattern("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")
			]],
			password: ["", [
				Validators.required,
				Validators.minLength(6)
			]],
			passwordConfirm: ["", [
				Validators.required,
				Validators.minLength(6)
			]],
			displayName: ["", null]
		},
			{
				validator: this.compareValidator('password', 'passwordConfirm')
			})
	}

	compareValidator(fc1: string, fc2: string) {
		return (group: FormGroup): { [key: string]: any } => {
			let password = group.controls[fc1]
			let passwordConfirm = group.controls[fc2]

			if (password.value === passwordConfirm.value) return null
			return { compareFailed: true }
		}

	}

	onSubmit() {
		this.authService.add(this.userForm.value)
			.subscribe(data => {
				if (data == null) {
					// registration successful
					this.erroMessage = null;
					this.authService.login(
						this.userForm.value.username,
						this.userForm.value.password)
						.subscribe(
						data => {
							// login successful
							this.erroMessage = null;
							this.router.navigate([""]);
						},
						err => {
							console.log(err)
							this.erroMessage = "Warning: Username or Password mismatch"
						})
				}
				else {
					// registration failure
					this.erroMessage = data.error;
				}
			},
			error => {
				// Server/Connection error
				this.erroMessage = error
			})
	}
}
