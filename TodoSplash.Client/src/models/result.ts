export class Result<T> {
    public readonly success: boolean;
    public readonly data?: T;
    public readonly error?: string;

    private constructor(success: boolean, data?: T, error?: string) {
        this.success = success;
        this.data = data;
        this.error = error;
    }

    public static Success<T>(data: T): Result<T> {
        return new Result<T>(true, data);
    }

    public static Failure<T>(error: string): Result<T> {
        return new Result<T>(false, undefined, error);
    }
}
