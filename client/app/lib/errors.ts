export function getErrorMessage(error: unknown) {
    console.error('[THIS IS AN ERROR] ' + error)

    if (error instanceof TypeError) {
        return 'Unable to connect to the server. Please try again later.'
    }

    if (error instanceof Error) {
        return error.message
    }

    return 'Something went wrong. Please try again later.'
}
