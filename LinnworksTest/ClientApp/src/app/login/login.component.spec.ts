import {HttpClient} from '@angular/common/http';
import {TestBed} from '@angular/core/testing';
import {Router} from '@angular/router';
import {LoginComponent} from './login.component';


describe('LoginComponent unit tests', () => {
  let router: Router;
  let httpClient: HttpClient;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LoginComponent]
    });
  });

  it('LoginComponent constructor initialization', () => {
    const loginComponent: LoginComponent = new LoginComponent(httpClient, router);

    expect(loginComponent).toBeDefined();
    expect(loginComponent.isRequesting).toBeUndefined();
    expect(loginComponent.token).toBeUndefined();
    expect(loginComponent.errors).toBeUndefined();
  });

  it('`isRequesting` property is defined correctly', () => {
    const component: LoginComponent = new LoginComponent(httpClient, router);

    component.isRequesting = false;

    expect(component.isRequesting).toBeDefined();
    expect(component.isRequesting).toEqual(jasmine.any(Boolean));
    expect(component.isRequesting).toBe(false);
  });

  it('`token` property is defined correctly', () => {
    const loginComponent: LoginComponent = new LoginComponent(httpClient, router);

    loginComponent.token = "bccf905c-6592-40f2-8db1-c976791fa40a";

    expect(loginComponent.token).toBeDefined();
    expect(loginComponent.token).toEqual(jasmine.any(String));
    expect(loginComponent.token).toBe("bccf905c-6592-40f2-8db1-c976791fa40a");
  });

  it('`errors` property is defined correctly', () => {
    const loginComponent: LoginComponent = new LoginComponent(httpClient, router);

    loginComponent.errors = ["Invalid token."];

    expect(loginComponent.errors).toBeDefined();
    expect(loginComponent.errors).toEqual(jasmine.arrayContaining(["Invalid token."]));
  });

});
