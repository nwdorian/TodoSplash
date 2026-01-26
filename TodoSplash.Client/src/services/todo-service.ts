import type { TodoRead } from "../models/todo-read";
import type { TodoCreate } from "../models/todo-create";
import type { TodoUpdate } from "../models/todo-update";
import { Result } from "../models/result";
import { API_ROUTES } from "../constants/api-routes";

const TodoService = {
    getAll: async (): Promise<Result<TodoRead[]>> => {
        try {
            const response = await fetch(API_ROUTES.TODOS, {
                method: "GET",
                headers: {
                    Accept: "application/json",
                },
            });

            if (!response.ok) {
                const error = await response.text();
                return Result.Failure<TodoRead[]>(error || "Failed to fetch todo items.");
            }

            const data: TodoRead[] = await response.json();
            return Result.Success<TodoRead[]>(data);
        } catch (error) {
            return Result.Failure<TodoRead[]>("An error occurred.");
        }
    },
    create: async (todo: TodoCreate): Promise<Result<TodoRead>> => {
        try {
            const response = await fetch(API_ROUTES.TODOS, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(todo),
            });

            if (!response.ok) {
                const error = await response.text();
                return Result.Failure<TodoRead>(error || "Failed to create a todo item.");
            }

            const data: TodoRead = await response.json();
            return Result.Success<TodoRead>(data);
        } catch (error) {
            return Result.Failure<TodoRead>("An error occurred.");
        }
    },
    update: async (id: number, todo: TodoUpdate): Promise<Result<void>> => {
        const response = await fetch(`${API_ROUTES.TODOS}/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(todo),
        });

        if (!response.ok) {
            const error = await response.text();
            return Result.Failure<void>(error || "Failed to update todo item.");
        }

        return Result.Success(undefined);
    },
    delete: async (id: number): Promise<Result<void>> => {
        const response = await fetch(`${API_ROUTES.TODOS}/${id}`, {
            method: "DELETE",
        });

        if (!response.ok) {
            const error = await response.text();
            return Result.Failure<void>(error || "Failed to delete todo item.");
        }

        return Result.Success(undefined);
    },
};

export default TodoService;
