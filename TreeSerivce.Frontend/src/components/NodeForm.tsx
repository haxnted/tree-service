import React, { useState } from "react";
import { createNode, updateNode } from "../shared/api/api";
import Modal from "./Modal";

interface NodeFormProps {
	isOpen: boolean;
	onClose: () => void;
	initialData?: {
		id?: string;
		title: string;
		description: string;
		parentId?: string;
	};
	onSuccess: () => void;
}

const NodeForm: React.FC<NodeFormProps> = ({
	isOpen,
	onClose,
	initialData,
	onSuccess,
}) => {
	const [title, setTitle] = useState(initialData?.title || "");
	const [description, setDescription] = useState(
		initialData?.description || ""
	);

	const handleSubmit = async () => {
		try {
			if (initialData?.id) {
				await updateNode(initialData.id, { title, description });
			} else {
				await createNode({
					title,
					description,
					parentId: initialData?.parentId,
				});
			}
			onSuccess();
			onClose();
		} catch (error) {
			console.error(error);
		}
	};

	return (
		<Modal
			isOpen={isOpen}
			onClose={onClose}
			title={initialData?.id ? "Edit Node" : "Create Node"}
		>
			<form onSubmit={(e) => e.preventDefault()}>
				<div className="mt-4">
					<label className="block text-sm font-medium text-gray-700">
						Title
					</label>
					<input
						type="text"
						value={title}
						onChange={(e) => setTitle(e.target.value)}
						className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-200"
					/>
				</div>
				<div className="mt-4">
					<label className="block text-sm font-medium text-gray-700">
						Description
					</label>
					<textarea
						value={description}
						onChange={(e) => setDescription(e.target.value)}
						className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-200"
					/>
				</div>
				<div className="mt-6">
					<button
						type="submit"
						onClick={handleSubmit}
						className="w-full px-4 py-2 text-sm font-medium text-white bg-blue-600 rounded-md hover:bg-blue-700"
					>
						{initialData?.id ? "Update" : "Create"}
					</button>
				</div>
			</form>
		</Modal>
	);
};

export default NodeForm;
