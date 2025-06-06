import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';
import { AngularFireAuth } from '@angular/fire/compat/auth';
import { Router } from '@angular/router';
import { of } from 'rxjs';

describe('AuthService', () => {
  let service: AuthService;
  let angularFireAuthMock: any;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(() => {
    angularFireAuthMock = jasmine.createSpyObj('AngularFireAuth', ['signInWithPopup', 'signOut']);
    angularFireAuthMock.authState = of(null);

    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        AuthService,
        { provide: AngularFireAuth, useValue: angularFireAuthMock },
        { provide: Router, useValue: routerSpy },
        { provide: 'angularfire2.app.options', useValue: {} },
      ],
    });

    service = TestBed.inject(AuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should navigate to /reservation after successful Google sign-in', async () => {
    angularFireAuthMock.signInWithPopup.and.returnValue(Promise.resolve());

    await service.signInWithGoogle();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/reservation']);
  });

  it('should navigate to /login after signing out', async () => {
    angularFireAuthMock.signOut.and.returnValue(Promise.resolve());

    await service.signOut();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should return false for isLoggedIn when authState is null', (done) => {
    angularFireAuthMock.authState = of(null); 

    service.isLoggedIn().subscribe((isLoggedIn) => {
      expect(isLoggedIn).toBeFalse();
      done();
    });
  });
});