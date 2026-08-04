// app/routes/login.tsx

import type { Route } from '../+types'
import type { LoginRequest } from '~/types/auth'

import { Form, Link, useActionData, redirect } from 'react-router'
import { useState } from 'react'
import { AuthApi } from '~/lib/api/auth.server'
import { copySetCookieHeaders } from '~/lib/api/headers.server'
import { getErrorMessage } from '~/lib/errors'

export async function action({ request }: Route.ActionArgs) {
    const formData = await request.formData()

    const loginRequest: LoginRequest = {
        email: formData.get('email') as string,
        password: formData.get('password') as string,
    }

    try {
        const response = await AuthApi.login(loginRequest)

        if (!response.ok) {
            return { error: 'Invalid email or password' }
        }

        return redirect('/', {
            headers: copySetCookieHeaders(response),
        })
    } catch (error) {
        return {
            error: getErrorMessage(error),
        }
    }
}

export default function Login() {
    const actionData = useActionData<typeof action>()
    const [showPassword, setShowPassword] = useState(false)

    return (
        <div className="flex min-h-screen items-center justify-center bg-background">
            <div className="w-full max-w-md rounded-xl border border-border bg-card p-8 shadow-md">
                <h1 className="mb-2 text-center text-3xl font-bold text-text">
                    Welcome back!
                </h1>

                <p className="mb-6 text-center text-sm text-muted">
                    Don't have an account?{' '}
                    <Link
                        to="/register"
                        className="text-primary hover:underline"
                    >
                        Create account
                    </Link>
                </p>

                {actionData?.error && (
                    <div className="mb-4 rounded-lg bg-red-950 p-3 text-sm text-red-300">
                        {actionData.error}
                    </div>
                )}

                <Form
                    method="post"
                    className="space-y-4"
                >
                    <div>
                        <label
                            htmlFor="email"
                            className="mb-1 block text-sm font-medium text-text"
                        >
                            Email
                        </label>

                        <input
                            id="email"
                            name="email"
                            type="email"
                            placeholder="you@example.com"
                            className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text placeholder:text-muted outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
                            required
                        />
                    </div>

                    <div>
                        <label
                            htmlFor="password"
                            className="mb-1 block text-sm font-medium text-text"
                        >
                            Password
                        </label>

                        <div className="relative">
                            <input
                                id="password"
                                name="password"
                                type={showPassword ? 'text' : 'password'}
                                placeholder="Your password"
                                className="w-full rounded-lg border border-border bg-background px-3 py-2 pr-20 text-text placeholder:text-muted outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
                                required
                            />

                            <button
                                type="button"
                                onClick={() => setShowPassword(!showPassword)}
                                className="absolute right-3 top-1/2 -translate-y-1/2 text-sm text-primary hover:underline"
                            >
                                {showPassword ? 'Hide' : 'Show'}
                            </button>
                        </div>
                    </div>

                    <button
                        type="submit"
                        className="w-full rounded-lg bg-primary py-2 font-medium text-white transition hover:opacity-90"
                    >
                        Sign in
                    </button>
                </Form>
            </div>
        </div>
    )
}
