"use server";

import { cookies } from "next/headers";

const urlListRealms = `${process.env.API_URL}/UserTask`

export default async function CreateUserTask(request: any): Promise<any | null> {

  const bearerToken = cookies().get("token-jwt")?.value;

  const url = `${urlListRealms}`;

  const response = await fetch(
    url,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${bearerToken}`
      },
      body: JSON.stringify(request),
      next: {
        tags: ["create-user-task"]
      }
    }
  );

  if (!response?.ok) return null;

  return await response.json();
}
