import React from "react";

export interface NodeTree {
	id: string;
	parentId?: string | null;
	title: string;
	description: string;
	createdAt: string;
	childrenList?: NodeTree[] | null;
}

interface Props {
	node: NodeTree;
	onUpdate: (node: NodeTree) => void;
	onDelete: (id: string) => void;
}

export const NodeTree: React.FC<Props> = ({ node, onUpdate, onDelete }) => {
	const copyToClipboard = (text: string) => {
		navigator.clipboard
			.writeText(text)
			.then(() => alert("GUID скопирован в буфер обмена!"))
			.catch((err) => console.error("Ошибка копирования GUID:", err));
	};

	return (
		<div className="border rounded-lg p-4 bg-white shadow relative">
			<h2 className="text-xl font-semibold">{node.title || "Без заголовка"}</h2>
			<p className="text-gray-600">{node.description || "Нет описания"}</p>
			<p className="text-sm text-gray-400">GUID: {node.id}</p>

			{!node.parentId && (
				<p className="absolute top-2 right-2 text-sm text-red-500 font-bold">
					Нет родителей
				</p>
			)}

			<div className="flex gap-4 mt-4">
				<button
					className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
					onClick={() => onUpdate(node)}
				>
					Update
				</button>
				<button
					className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600"
					onClick={() => onDelete(node.id)}
				>
					Delete
				</button>
				<button
					className="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-600"
					onClick={() => copyToClipboard(node.id)}
				>
					Copy GUID
				</button>
			</div>

			{/* Отображение дочерних элементов */}
			{node.childrenList && node.childrenList.length > 0 ? (
				<div className="mt-4 pl-4 border-l border-gray-300">
					<h3 className="font-bold">Children:</h3>
					{node.childrenList.map((child, index) => (
						<div key={child.id || index} className="mt-2">
							<NodeTree node={child} onUpdate={onUpdate} onDelete={onDelete} />
						</div>
					))}
				</div>
			) : (
				<p className="text-gray-500">Нет дочерних элементов</p>
			)}
		</div>
	);
};
