import type { Route } from "../+types/root";

import { useState } from "react";
import { Link, Form, Outlet, redirect } from "react-router";
import { AuthApi } from "~/lib/api/auth.server";

import ThemeToggle from "~/components/ThemeToggle";

export async function loader({ request }: Route.LoaderArgs) {
    const user = await AuthApi.getCurrentUser(request);

    if (!user) return redirect("/login");

    return null;
}

export default function ProtectedLayout() {
    const [menuOpen, setMenuOpen] = useState(false);

    return (
        <div className="min-h-screen bg-background text-text">
            <nav className="border-b border-border bg-card">
                <div className="flex h-16 items-center justify-between px-4">
                    {/* Brand */}
                    <Link to="/">
                        <h1 className="text-xl font-bold text-primary">
                            GarageLog
                        </h1>
                    </Link>

                    {/* Desktop Menu */}
                    <div className="hidden items-center gap-4 md:flex">
                        <span className="text-sm text-muted">
                            userName#todo
                        </span>

                        <ThemeToggle />
                        <Form method="post" action="/logout">
                            <button className="rounded-lg bg-primary px-4 py-2 text-sm font-medium text-white transition hover:opacity-90">
                                Log out
                            </button>
                        </Form>
                    </div>

                    {/* Mobile Actions */}
                    <div className="flex items-center gap-3 md:hidden">
                        {/* Hamburger Button */}
                        <button
                            className="text-2xl text-muted"
                            onClick={() => setMenuOpen(!menuOpen)}
                        >
                            {menuOpen ? "✕" : "☰"}
                        </button>
                    </div>
                </div>

                {/* Mobile Menu */}
                {menuOpen && (
                    <div className="border-t border-border bg-card md:hidden">
                        <div className="flex flex-col items-start gap-4 p-4">
                            <span className="text-sm text-muted">
                                userName#todo
                            </span>

                            <Form method="post" action="/logout">
                                <span className="text-sm text-muted">
                                    Log out
                                </span>
                            </Form>
                            <ThemeToggle />
                        </div>
                    </div>
                )}
            </nav>

            {/* Page Content */}
            <main className="p-4 md:p-8">
                <Outlet />
            </main>
        </div>
    );
}
