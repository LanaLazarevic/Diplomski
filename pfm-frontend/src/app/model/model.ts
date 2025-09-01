export interface TransactionDto {
  id: string;
  date: string;
  direction: string;
  amount: number;
  beneficiaryName?: string;
  description?: string;
  currency: string;
  mcc?: string;
  kind: string;
  catCode?: string;
  cardNumber?: string;
  splits: SplitItemDto[];
}

export interface SplitItemDto {
  catCode: string;
  amount: number;
}

export interface PagedList<T> {
  items: T[];
  totalCount: number;
  pageSize: number;
  page: number;
  totalPages: number;
  sortOrder: string;
  sortBy: string;
}

export interface TransactionDtoRaw {
  id: string;
  date: string;
  direction: string;
  amount: number;
  'beneficiary-name'?: string;
  description?: string;
  currency: string;
  mcc?: string;
  kind: string;
  catcode?: string;
  'card-number'?: string;
  splits: SplitItemDtoRaw[];
}

export interface PagedListRaw<T> {
  items: T[];
  'total-count': number;
  'page-size': number;
  page: number;
  'total-pages': number;
  'sort-order': string;
  'sort-by': string;
}

export interface SplitItemDtoRaw {
  catcode: string;
  amount: number;
}

export interface FilterParams {
  'transaction-kind'?: string[];
  'start-date'?: string;
  'end-date'?: string;
  'sort-by'?: string;
  'sort-order'?: string;
  'catcode'?: string;
}

export interface CategoryDto {
  'parent-code': string;
  code: string;
  name: string;
}

export interface CategoriesResponse {
  items: CategoryDto[];
}

export interface LoginResponse {
  jwt: string;
}

export interface BusinessError {
  problem: string;
  message: string;
  details: string;
}

export interface Split {
  catcode: string;
  subcatcode: string;
  amount: number;
}

export interface SpendingAnalyticsGroup {
  catcode: string;
  amount: number;
  count: number;
}

export interface SpendingAnalyticsResponse {
  groups: SpendingAnalyticsGroup[];
}

export interface SpendingAnalyticsItem {
  catcode: string;
  name: string;
  percentage: number;
}

export interface CardDto {
  id: string;
  ownerName: string;
  cardNumber: string;
  expirationDate: string;
  availableAmount: number;
  reservedAmount: number;
  cardType: string;
  userId: string;
  isActive: boolean;
}

export interface CardDtoRaw {
  id: string;
  'owner-name': string;
  'card-number': string;
  'expiration-date': string;
  'available-amount': number;
  'reserved-amount': number;
  'card-type': string;
  'user-id': string;
  'is-active': boolean;
}
