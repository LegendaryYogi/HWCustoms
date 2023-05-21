export interface Pagination<T> {   //84.
    pageIndex: number;
    pageSize: number;
    count: number;
    data: T;
  }