// app/routes/register.tsx

import type { Route } from '../+types'
import type { RegisterRequest } from '~/types/auth'

import {
    Form,
    Link,
    useActionData,
    redirect,
    useNavigation,
} from 'react-router'
import { useState } from 'react'
import { AuthApi } from '~/lib/api/auth.server'
import { copySetCookieHeaders } from '~/lib/api/headers.server'
import { getErrorMessage } from '~/lib/errors'

export async function action({ request }: Route.ActionArgs) {
    const formData = await request.formData()

    const registerRequest: RegisterRequest = {
        firstName: formData.get('firstName') as string,
        lastName: formData.get('lastName') as string,
        email: formData.get('email') as string,
        password: formData.get('password') as string,
    }

    try {
        const response = await AuthApi.register(registerRequest)

        if (!response.ok) {
            const error = await response.json()

            return {
                error:
                    Object.values(error.errors ?? {})
                        .flat()
                        .join(' ') ||
                    error.error ||
                    error.message ||
                    'Something went wrong',
            }
        }

        return redirect('/', {
            headers: copySetCookieHeaders(response),
        })
    } catch (error) {
        if (error instanceof TypeError) {
            return {
                error: 'Unable to connect to the server. Please try again later.',
            }
        }
        if (error instanceof Error) {
            return {
                error: error.message,
            }
        }
        return {
            error: getErrorMessage(error),
        }
    }
}

export default function Register() {
    const actionData = useActionData<typeof action>()
    const [showPassword, setShowPassword] = useState(false)

    const navigation = useNavigation()

    const isSubmitting = navigation.state === 'submitting'

    return (
        <div className="flex min-h-screen items-center justify-center bg-background">
            <div className="w-full max-w-md rounded-xl border border-border bg-card p-8 shadow-md">
                <h1 className="mb-2 text-center text-3xl font-bold text-text">
                    Create account
                </h1>

                <p className="mb-6 text-center text-sm text-muted">
                    Already have an account?{' '}
                    <Link
                        to="/login"
                        className="text-primary hover:underline"
                    >
                        Sign in
                    </Link>
                </p>

                {actionData?.error && (
                    <div className="mx-auto mb-6 max-w-3xl rounded-lg border border-red-500/30 bg-red-500/10 p-4 text-sm text-red-500">
                        {actionData.error}
                    </div>
                )}

                <Form
                    method="post"
                    className="space-y-4"
                >
                    <div>
                        <label className="mb-1 block text-sm font-medium text-text">
                            First name
                        </label>

                        <input
                            name="firstName"
                            type="text"
                            placeholder="John"
                            className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text placeholder:text-muted outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
                            required
                        />
                    </div>

                    <div>
                        <label className="mb-1 block text-sm font-medium text-text">
                            Last name
                        </label>

                        <input
                            name="lastName"
                            type="text"
                            placeholder="Doe"
                            className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text placeholder:text-muted outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
                            required
                        />
                    </div>

                    <div>
                        <label className="mb-1 block text-sm font-medium text-text">
                            Email
                        </label>

                        <input
                            name="email"
                            type="email"
                            placeholder="you@example.com"
                            className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text placeholder:text-muted outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
                            required
                        />
                    </div>

                    <div>
                        <label className="mb-1 block text-sm font-medium text-text">
                            Password
                        </label>

                        <div className="relative">
                            <input
                                name="password"
                                type={showPassword ? 'text' : 'password'}
                                placeholder="Create a password"
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
                        disabled={isSubmitting}
                        className="w-full rounded-lg bg-primary px-4 py-2 font-medium text-white transition disabled:cursor-not-allowed disabled:opacity-50"
                    >
                        {isSubmitting ? 'Registering...' : 'Register'}
                    </button>
                </Form>
            </div>
        </div>
    )
}
