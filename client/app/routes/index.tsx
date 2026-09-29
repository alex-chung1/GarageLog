import type { Route } from './+types/index';
import type { UserResponse } from '~/types/auth';

import { Link } from 'react-router';
import { AuthApi } from '~/lib/api/auth.server';
import ThemeToggle from '~/components/ThemeToggle';

export async function loader({ request }: Route.LoaderArgs) {
  let user: UserResponse | null = null;

  try {
    const response = await AuthApi.getCurrentUser(request);

    if (response.ok) {
      user = await response.json();
    }
  } catch {
    user = null;
  }

  return { user };
}

export default function Home({ loaderData }: Route.ComponentProps) {
  const { user } = loaderData;

  return (
    <div className="min-h-screen bg-background text-text">
      {/* Navbar */}
      <nav className="fixed inset-x-0 top-0 z-50 border-b border-border/50 bg-card/75 backdrop-blur-md">
        <div className="mx-auto flex h-18 max-w-9xl items-center justify-between px-4 md:px-6">
          {/* Logo */}
          <Link
            to="/"
            className="text-xl font-bold tracking-tight text-primary transition-opacity hover:opacity-80"
          >
            GarageLog
          </Link>

          {/* Navigation */}
          <div className="flex items-center gap-2">
            {user ? (
              <>
                <span className="hidden px-3 text-sm text-muted sm:block">
                  Hello, {user.firstName}
                </span>

                <Link
                  to="/garage"
                  className="rounded-lg bg-primary px-4 py-2 text-sm font-medium text-white shadow-sm transition hover:opacity-90"
                >
                  My Garage
                </Link>
              </>
            ) : (
              <>
                <Link
                  to="/login"
                  className="rounded-lg px-4 py-2 text-sm font-medium text-muted transition hover:bg-background hover:text-text"
                >
                  Log in
                </Link>

                <Link
                  to="/register"
                  className="rounded-lg bg-primary px-4 py-2 text-sm font-medium text-white shadow-sm transition hover:opacity-90"
                >
                  Get Started
                </Link>
              </>
            )}

            <div className="ml-1 border-l border-border/60 pl-2">
              <ThemeToggle />
            </div>
          </div>
        </div>
      </nav>

      {/* Main */}
      <main>
        {/* Hero */}
        <section className="relative isolate overflow-hidden">
          <div className="relative min-h-[560px] sm:min-h-[620px] lg:min-h-[680px]">
            <img
              src="/images/hero-car.jpg"
              alt=""
              className="absolute inset-0 -z-20 h-full w-full object-cover"
            />

            <div className="absolute inset-0 -z-10 bg-background/80" />

            <div className="mx-auto flex min-h-[560px] max-w-7xl items-center justify-center px-4 py-20 text-center sm:min-h-[620px] sm:px-6 sm:py-24 lg:min-h-[680px]">
              <div>
                <p className="mb-4 text-sm font-semibold uppercase tracking-wider text-primary">
                  Garage management made simple
                </p>

                <h1 className="text-5xl font-bold tracking-tight sm:text-6xl md:text-7xl">
                  Your garage.
                  <br />
                  <span className="text-primary">One place.</span>
                </h1>

                <p className="mx-auto mt-6 max-w-2xl text-lg text-muted sm:text-xl">
                  Keep track of your vehicles, maintenance, and service history without digging
                  through receipts and notes.
                </p>

                <div className="mt-8">
                  <Link
                    to={user ? '/garage' : '/register'}
                    className="inline-block rounded-lg bg-primary px-6 py-3 font-medium text-white shadow-lg transition hover:opacity-90"
                  >
                    {user ? 'Go to My Garage' : 'Get Started'}
                  </Link>
                </div>
              </div>
            </div>
          </div>
        </section>

        {/* Features */}
        <section className="border-t border-border bg-background">
          <div className="mx-auto max-w-7xl px-4 py-20 md:px-6 md:py-28">
            <div className="mx-auto max-w-2xl text-center">
              <p className="text-sm font-semibold uppercase tracking-wider text-primary">
                Everything in one place
              </p>

              <h2 className="mt-3 text-3xl font-bold tracking-tight md:text-4xl">
                Built for your garage
              </h2>

              <p className="mt-4 text-lg text-muted">
                Keep your vehicles organized and your maintenance history easy to find.
              </p>
            </div>

            <div className="mt-12 grid gap-6 md:grid-cols-3">
              {/* Feature 1 */}
              <div className="rounded-2xl border border-border bg-card p-6 transition hover:-translate-y-1 hover:border-primary/40">
                <div className="mb-5 flex h-11 w-11 items-center justify-center rounded-xl bg-primary/10 text-primary">
                  🚗
                </div>

                <h3 className="text-lg font-semibold">Manage Your Vehicles</h3>

                <p className="mt-2 text-sm leading-6 text-muted">
                  Keep all your vehicles organized with details like mileage, year, make, model, and
                  more.
                </p>
              </div>

              {/* Feature 2 */}
              <div className="rounded-2xl border border-border bg-card p-6 transition hover:-translate-y-1 hover:border-primary/40">
                <div className="mb-5 flex h-11 w-11 items-center justify-center rounded-xl bg-primary/10 text-primary">
                  🔧
                </div>

                <h3 className="text-lg font-semibold">Track Service History</h3>

                <p className="mt-2 text-sm leading-6 text-muted">
                  Record repairs and maintenance so you always know what was done and when.
                </p>
              </div>

              {/* Feature 3 */}
              <div className="rounded-2xl border border-border bg-card p-6 transition hover:-translate-y-1 hover:border-primary/40">
                <div className="mb-5 flex h-11 w-11 items-center justify-center rounded-xl bg-primary/10 text-primary">
                  📋
                </div>

                <h3 className="text-lg font-semibold">Stay Organized</h3>

                <p className="mt-2 text-sm leading-6 text-muted">
                  Keep important vehicle information in one place instead of scattered across notes,
                  receipts, and apps.
                </p>
              </div>
            </div>
          </div>
        </section>
      </main>
    </div>
  );
}
