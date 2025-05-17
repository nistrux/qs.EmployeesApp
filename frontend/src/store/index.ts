import { writable } from 'svelte/store';
import type { DepartmentDto, PositionDto } from '$types';

export const departments = writable<DepartmentDto[]>([]);
export const positions = writable<PositionDto[]>([]);