export default function FormSection({
    title,
    children,
}: {
    title: string
    children: React.ReactNode
}) {
    return (
        <section className="rounded-xl border border-border bg-card p-6 shadow-sm">
            <h2 className="mb-4 text-xl font-semibold text-text">{title}</h2>

            {children}
        </section>
    )
}
