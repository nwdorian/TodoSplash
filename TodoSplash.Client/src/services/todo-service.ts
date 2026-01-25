import type { TodoRead } from "../models/todo-read";
import { API_ROUTES } from "../constants/api-routes";

export const TodoService = {
    async getAll(): Promise<TodoRead[]> {
        try {
            const response = await fetch(API_ROUTES.TODOS, {
                method: "GET",
                headers: {
                    Accept: "application/json",
                },
            });

            if (!response.ok) {
                throw new Error(`HTTP Error! Status: ${response.status}`);
            }

            const data: TodoRead[] = await response.json();
            return data;
        } catch (error) {
            console.error("Failed to fetch todos in TodoService:", error);

            throw error;
        }
    },
};
