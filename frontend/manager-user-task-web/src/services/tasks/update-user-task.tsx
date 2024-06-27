"use server";

import { cookies } from "next/headers";

const urlListRealms = `${process.env.API_URL}/UserTask`

export default async function UpdateUserTask(id: any, request: any): Promise<any | null> {

  const bearerToken = cookies().get("token-jwt")?.value;

  const url = `${urlListRealms}/${id}`;

  const response = await fetch(
    url,
    {
      method: "PUT",
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

  if (!response?.ok) return false;

  return true;
}
