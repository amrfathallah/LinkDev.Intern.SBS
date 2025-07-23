# Shared Module

This folder contains reusable components, models, and services that can be used across the application.

## Structure

```
shared/
├── models/
│   ├── api-response.model.ts     # Generic API response interface(pagination, errors)
│   └── index.ts                  # Barrel export for models
├── services/
│   ├── base-api.service.ts       # Base service with common HTTP operations
│   └── index.ts                  # Barrel export for services
└── index.ts                      # Main barrel export
```

## Usage

### API Response

The `ApiResponse<T>` interface is a generic type that provides a consistent structure for all API responses:

```typescript
import { ApiResponse } from '../../shared';
import { AuthResponse } from '../models/auth-response.model';

// Usage in service
login(data: LoginRequest): Observable<ApiResponse<AuthResponse>> {
  return this.http.post<ApiResponse<AuthResponse>>('/auth/login', data);
}
```

### Base API Service

Extend the `BaseApiService` for consistent HTTP operations:

```typescript
import { BaseApiService } from "../../shared";

@Injectable()
export class UserService extends BaseApiService {
  protected baseUrl = "https://api.example.com/users";

  getUser(id: number): Observable<ApiResponse<User>> {
    return this.get<User>(`/${id}`);
  }
}
```

## Benefits

- **Type Safety**: Generic types ensure compile-time type checking
- **Consistency**: All API responses follow the same structure
- **Reusability**: Shared components reduce code duplication
- **Maintainability**: Changes to common types only need to be made in one place
