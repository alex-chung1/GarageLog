import type { Route } from '../+types/root';
import type { UserResponse } from '~/types/auth';

import { useState } from 'react';
import { Link, Form, Outlet, redirect, useLoaderData, useNavigation } from 'react-router';
import { AuthApi } from '~/lib/api/auth.server';

export async function loader({ request }: Route.LoaderArgs) {
  try {
    const response = await AuthApi.getCurrentUser(request);

    if (!response.ok) return redirect('/login');

    const user: UserResponse = await response.json();

    return { user };
  } catch {
    return redirect('/login');
  }
}

export default function ProtectedLayout() {
  const { user } = useLoaderData<typeof loader>();

  const navigation = useNavigation();
  const isSubmitting = navigation.state === 'submitting';

  const [menuOpen, setMenuOpen] = useState(false);

  return (
    <div className="flex min-h-screen flex-col bg-background text-text">
      {/* Global Submit Loading Overlay */}
      {isSubmitting && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-sm">
          <div className="rounded-xl border border-border bg-card px-8 py-6 shadow-lg">
            <div className="flex flex-col items-center gap-4">
              <div className="h-8 w-8 animate-spin rounded-full border-4 border-primary border-t-transparent" />

              <p className="font-medium text-text">Saving...</p>

              <p className="text-sm text-muted">Please wait</p>
            </div>
          </div>
        </div>
      )}

      {/* Navbar */}
      <nav className="fixed inset-x-0 top-0 z-40 border-b border-border/50 bg-card/75 backdrop-blur-md">
        <div className="mx-auto flex h-18 max-w-9xl items-center justify-between px-4 md:px-6">
          {/* Brand */}
          <Link
            to="/garage"
            className="text-xl font-bold tracking-tight text-primary transition-opacity hover:opacity-80"
          >
            GarageLog
          </Link>

          {/* Desktop Menu */}
          <div className="hidden items-center gap-2 md:flex">
            <span className="px-3 text-sm text-muted">Welcome back, {user.firstName}</span>

            <Form method="post" action="/logout">
              <button
                type="submit"
                className="rounded-lg border border-border bg-card/60 px-4 py-2 text-sm font-medium text-text shadow-sm transition hover:border-primary hover:text-primary"
              >
                Log out
              </button>
            </Form>
          </div>

          {/* Mobile Actions */}
          <div className="flex items-center gap-2 md:hidden">
            <button
              type="button"
              aria-label="Toggle menu"
              aria-expanded={menuOpen}
              onClick={() => setMenuOpen(!menuOpen)}
              className="rounded-lg px-3 py-2 text-xl text-muted transition hover:bg-background hover:text-text"
            >
              {menuOpen ? '✕' : '☰'}
            </button>
          </div>
        </div>

        {/* Mobile Menu */}
        {menuOpen && (
          <div className="border-t border-border/50 bg-card/95 backdrop-blur-md md:hidden">
            <div className="flex items-center justify-between px-4 py-5">
              {/* User */}
              <div>
                <p className="text-sm text-muted">Signed in as</p>
                <p className="mt-1 font-medium text-text">{user.firstName}</p>
              </div>

              {/* Actions */}
              <div className="flex items-center gap-3">
                <Form method="post" action="/logout">
                  <button
                    type="submit"
                    className="rounded-lg border border-border bg-card/60 px-4 py-2 text-sm font-medium text-text shadow-sm transition hover:border-primary hover:text-primary"
                  >
                    Log out
                  </button>
                </Form>
              </div>
            </div>
          </div>
        )}
      </nav>

      {/* Page Content */}
      <main className="flex-1 pt-18">
        <div className="p-4 md:p-8">
          <Outlet />
        </div>
      </main>

      {/* Footer */}
      <footer className="px-4 py-6 text-center text-sm text-muted">
        Copyright © 2026 Alexander Chung
      </footer>
    </div>
  );
}
