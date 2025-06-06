import { HttpHeaders, HttpParams } from "@angular/common/http";

export interface Options{
    headers?: HttpHeaders;
    params?: HttpParams;
    body?: unknown;
}
