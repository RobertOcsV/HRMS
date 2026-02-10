# Setup do Frontend Angular - HRMS

## Pre-requisitos

- Node.js 20+ (https://nodejs.org/)
- npm ou pnpm
- Angular CLI 19

## Passo 1: Instalar Angular CLI

```bash
npm install -g @angular/cli@19
```

Verifique a instalacao:
```bash
ng version
```

## Passo 2: Criar o Projeto

Navegue ate a pasta frontend e crie o projeto:

```bash
cd D:\Versionados_Projetos\HRMS\frontend

ng new hrms-web --routing --style=scss --ssr=false --skip-tests
```

Opcoes selecionadas:
- `--routing` - Adiciona modulo de rotas
- `--style=scss` - Usa SCSS para estilos
- `--ssr=false` - Sem Server-Side Rendering (simplifica)
- `--skip-tests` - Pula criacao de testes iniciais (adicionaremos depois)

## Passo 3: Instalar Dependencias

```bash
cd hrms-web

# Angular Material
ng add @angular/material

# Quando perguntado:
# - Theme: Custom (ou Indigo/Pink)
# - Typography: Yes
# - Animations: Yes

# Outras dependencias uteis
npm install ngx-charts --save
npm install date-fns --save
```

## Passo 4: Estrutura de Pastas

Crie a estrutura de pastas recomendada:

```bash
cd src/app

# Core - servicos singleton, guards, interceptors
mkdir -p core/services
mkdir -p core/guards
mkdir -p core/interceptors
mkdir -p core/models

# Shared - componentes reutilizaveis
mkdir -p shared/components
mkdir -p shared/pipes
mkdir -p shared/directives

# Features - modulos de funcionalidades
mkdir -p features/auth/pages
mkdir -p features/auth/components
mkdir -p features/dashboard/pages
mkdir -p features/colaboradores/pages
mkdir -p features/colaboradores/components

# Layout - estrutura visual
mkdir -p layout/components
```

## Passo 5: Configurar Environments

Edite `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api',
  tokenKey: 'hrms_token',
  refreshTokenKey: 'hrms_refresh_token'
};
```

Crie `src/environments/environment.prod.ts`:

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.seudominio.com/api',
  tokenKey: 'hrms_token',
  refreshTokenKey: 'hrms_refresh_token'
};
```

## Passo 6: Configurar angular.json

Adicione os environments no `angular.json`:

```json
"configurations": {
  "production": {
    "fileReplacements": [
      {
        "replace": "src/environments/environment.ts",
        "with": "src/environments/environment.prod.ts"
      }
    ],
    ...
  }
}
```

## Passo 7: Criar Servicos Base

### AuthService

Crie `src/app/core/services/auth.service.ts`:

```typescript
import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';

export interface LoginRequest {
  email: string;
  senha: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  usuario: {
    id: string;
    nome: string;
    email: string;
    role: string;
  };
}

export interface User {
  id: string;
  nome: string;
  email: string;
  role: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiUrl = environment.apiUrl;

  currentUser = signal<User | null>(null);
  isAuthenticated = signal<boolean>(false);

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    this.loadUserFromStorage();
  }

  login(credentials: LoginRequest) {
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, credentials);
  }

  handleLoginSuccess(response: LoginResponse) {
    localStorage.setItem(environment.tokenKey, response.accessToken);
    localStorage.setItem(environment.refreshTokenKey, response.refreshToken);
    localStorage.setItem('user', JSON.stringify(response.usuario));

    this.currentUser.set(response.usuario);
    this.isAuthenticated.set(true);
  }

  logout() {
    localStorage.removeItem(environment.tokenKey);
    localStorage.removeItem(environment.refreshTokenKey);
    localStorage.removeItem('user');

    this.currentUser.set(null);
    this.isAuthenticated.set(false);

    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(environment.tokenKey);
  }

  private loadUserFromStorage() {
    const token = this.getToken();
    const userJson = localStorage.getItem('user');

    if (token && userJson) {
      try {
        const user = JSON.parse(userJson);
        this.currentUser.set(user);
        this.isAuthenticated.set(true);
      } catch {
        this.logout();
      }
    }
  }
}
```

### API Interceptor

Crie `src/app/core/interceptors/auth.interceptor.ts`:

```typescript
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  if (token) {
    const cloned = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
    return next(cloned);
  }

  return next(req);
};
```

### Auth Guard

Crie `src/app/core/guards/auth.guard.ts`:

```typescript
import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticated()) {
    return true;
  }

  router.navigate(['/login']);
  return false;
};
```

## Passo 8: Configurar App Config

Edite `src/app/app.config.ts`:

```typescript
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

import { routes } from './app.routes';
import { authInterceptor } from './core/interceptors/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideAnimationsAsync()
  ]
};
```

## Passo 9: Configurar Rotas

Edite `src/app/app.routes.ts`:

```typescript
import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/auth/pages/login/login.component')
      .then(m => m.LoginComponent)
  },
  {
    path: '',
    canActivate: [authGuard],
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        loadComponent: () => import('./features/dashboard/pages/dashboard/dashboard.component')
          .then(m => m.DashboardComponent)
      },
      {
        path: 'colaboradores',
        loadComponent: () => import('./features/colaboradores/pages/colaboradores-list/colaboradores-list.component')
          .then(m => m.ColaboradoresListComponent)
      }
    ]
  },
  {
    path: '**',
    redirectTo: ''
  }
];
```

## Passo 10: Criar Pagina de Login

Crie `src/app/features/auth/pages/login/login.component.ts`:

```typescript
import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="login-container">
      <mat-card class="login-card">
        <mat-card-header>
          <mat-card-title>HRMS</mat-card-title>
          <mat-card-subtitle>Sistema de Gestao de RH</mat-card-subtitle>
        </mat-card-header>

        <mat-card-content>
          <form [formGroup]="form" (ngSubmit)="onSubmit()">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Email</mat-label>
              <input matInput formControlName="email" type="email" />
              <mat-icon matSuffix>email</mat-icon>
              @if (form.get('email')?.hasError('required') && form.get('email')?.touched) {
                <mat-error>Email e obrigatorio</mat-error>
              }
              @if (form.get('email')?.hasError('email') && form.get('email')?.touched) {
                <mat-error>Email invalido</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Senha</mat-label>
              <input matInput formControlName="senha"
                     [type]="hidePassword() ? 'password' : 'text'" />
              <button mat-icon-button matSuffix type="button"
                      (click)="hidePassword.set(!hidePassword())">
                <mat-icon>{{ hidePassword() ? 'visibility_off' : 'visibility' }}</mat-icon>
              </button>
              @if (form.get('senha')?.hasError('required') && form.get('senha')?.touched) {
                <mat-error>Senha e obrigatoria</mat-error>
              }
            </mat-form-field>

            @if (errorMessage()) {
              <div class="error-message">{{ errorMessage() }}</div>
            }

            <button mat-raised-button color="primary"
                    type="submit"
                    class="full-width"
                    [disabled]="loading()">
              @if (loading()) {
                <mat-spinner diameter="20"></mat-spinner>
              } @else {
                Entrar
              }
            </button>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .login-container {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: 100vh;
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    }

    .login-card {
      width: 100%;
      max-width: 400px;
      padding: 2rem;
    }

    .full-width {
      width: 100%;
      margin-bottom: 1rem;
    }

    .error-message {
      color: #f44336;
      margin-bottom: 1rem;
      text-align: center;
    }

    mat-card-header {
      justify-content: center;
      margin-bottom: 2rem;
    }

    mat-card-title {
      font-size: 2rem !important;
    }
  `]
})
export class LoginComponent {
  form: FormGroup;
  loading = signal(false);
  hidePassword = signal(true);
  errorMessage = signal('');

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      senha: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set('');

    this.authService.login(this.form.value).subscribe({
      next: (response) => {
        this.authService.handleLoginSuccess(response);
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        this.loading.set(false);
        this.errorMessage.set(
          error.error?.message || 'Erro ao fazer login. Tente novamente.'
        );
      }
    });
  }
}
```

## Passo 11: Criar Dashboard Placeholder

Crie `src/app/features/dashboard/pages/dashboard/dashboard.component.ts`:

```typescript
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  template: `
    <div class="dashboard-container">
      <h1>Dashboard</h1>
      <p>Bem-vindo, {{ authService.currentUser()?.nome }}!</p>

      <div class="cards-grid">
        <mat-card>
          <mat-card-header>
            <mat-card-title>Colaboradores</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <p class="stat-number">0</p>
          </mat-card-content>
        </mat-card>

        <mat-card>
          <mat-card-header>
            <mat-card-title>Ferias Pendentes</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <p class="stat-number">0</p>
          </mat-card-content>
        </mat-card>

        <mat-card>
          <mat-card-header>
            <mat-card-title>Avaliacoes</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <p class="stat-number">0</p>
          </mat-card-content>
        </mat-card>
      </div>
    </div>
  `,
  styles: [`
    .dashboard-container {
      padding: 2rem;
    }

    .cards-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 1rem;
      margin-top: 2rem;
    }

    .stat-number {
      font-size: 3rem;
      font-weight: bold;
      text-align: center;
      margin: 1rem 0;
    }
  `]
})
export class DashboardComponent {
  constructor(public authService: AuthService) {}
}
```

## Passo 12: Criar Lista de Colaboradores Placeholder

Crie `src/app/features/colaboradores/pages/colaboradores-list/colaboradores-list.component.ts`:

```typescript
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-colaboradores-list',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container">
      <h1>Colaboradores</h1>
      <p>Lista de colaboradores sera implementada aqui.</p>
    </div>
  `,
  styles: [`
    .container { padding: 2rem; }
  `]
})
export class ColaboradoresListComponent {}
```

## Passo 13: Rodar o Projeto

```bash
# Na pasta frontend/hrms-web
ng serve

# Acesse http://localhost:4200
```

## Proximos Passos

Apos a configuracao inicial, implemente:

1. [ ] Layout com sidebar e header
2. [ ] Servico de colaboradores
3. [ ] Tabela de colaboradores com paginacao
4. [ ] Formulario de cadastro/edicao
5. [ ] Tratamento de erros global
6. [ ] Loading states
7. [ ] Notificacoes (snackbar)

## Comandos Uteis

```bash
# Gerar componente
ng generate component features/colaboradores/components/colaborador-form

# Gerar servico
ng generate service core/services/colaborador

# Build de producao
ng build --configuration=production

# Rodar testes
ng test
```

---

Ultima atualizacao: 2026-02-10
