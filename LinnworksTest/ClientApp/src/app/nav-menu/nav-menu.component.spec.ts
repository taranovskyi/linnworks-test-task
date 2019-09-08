import {TestBed} from '@angular/core/testing';
import {Router} from '@angular/router';
import {CookieService} from 'ngx-cookie-service';
import {NavMenuComponent} from './nav-menu.component';

describe('NavMenuComponent unit tests', () => {
  let router: Router;
  let cookieService: CookieService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NavMenuComponent]
    });
  });

  it('NavMenuComponent constructor initialization', () => {
    const navMenuComponent: NavMenuComponent = new NavMenuComponent(cookieService, router);

    expect(navMenuComponent).toBeDefined();
    expect(navMenuComponent.isExpanded).toBeDefined();
    expect(navMenuComponent.isExpanded).toEqual(jasmine.any(Boolean));
    expect(navMenuComponent.isExpanded).toBe(false);
  });

  it('`collapse()` method set `isExpanded` to false', () => {
    const component: NavMenuComponent = new NavMenuComponent(cookieService, router);
    component.isExpanded = true;

    component.collapse();

    expect(component.isExpanded).toBe(false);
  });

  it('`toggle()` method set `isExpanded` to true', () => {
    const component: NavMenuComponent = new NavMenuComponent(cookieService, router);

    component.toggle();

    expect(component.isExpanded).toBe(true);
  });

});
