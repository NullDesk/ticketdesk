
export function toCamelCase(str: string) {
  if (str.length < 2) {
    throw new Error(`[utils.toCamelCase()] invalid length for ${str}`)
  }
  return str.charAt(0).toLowerCase()
    + str.slice(1).replace(/\s/g, '');
}

export function datePrettify(date: string) {
  
}